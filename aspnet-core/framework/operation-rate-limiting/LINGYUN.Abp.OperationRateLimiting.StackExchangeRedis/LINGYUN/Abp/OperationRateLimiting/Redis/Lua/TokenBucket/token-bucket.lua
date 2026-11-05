-- KEYS[1] = counter key (hash with tokens/last)
-- ARGV[1] = now (unix milliseconds)
-- ARGV[2] = replenishment period in milliseconds
-- ARGV[3] = token limit
-- ARGV[4] = tokens per period
-- ARGV[5] = consume (1/0)
-- ARGV[6] = ttl in milliseconds
-- returns { allowed, currentTokens, remaining, retryAfterMs }

local now = tonumber(ARGV[1])
local periodMs = tonumber(ARGV[2])
local tokenLimit = tonumber(ARGV[3])
local tokensPerPeriod = tonumber(ARGV[4])
local consume = ARGV[5] == '1'
local ttlMs = tonumber(ARGV[6])

local data = redis.call('HMGET', KEYS[1], 'tokens', 'last')
local tokens
local last

if not data[1] then
    tokens = tokenLimit
    last = now
else
    tokens = tonumber(data[1])
    last = tonumber(data[2])
end

local elapsed = now - last
if elapsed >= periodMs then
    local periods = math.floor(elapsed / periodMs)
    tokens = math.min(tokenLimit, tokens + periods * tokensPerPeriod)
    last = last + periods * periodMs
end

if tokens > tokenLimit then
    tokens = tokenLimit
end

local allowed = 0
if tokens >= 1 then
    allowed = 1
    if consume then
        tokens = tokens - 1
    end
end

redis.call('HMSET', KEYS[1], 'tokens', tokens, 'last', last)
redis.call('PEXPIRE', KEYS[1], ttlMs)

local retryMs = 0
if allowed == 0 then
    local missing = 1 - tokens
    local periodsNeeded = math.ceil(missing / tokensPerPeriod)
    retryMs = last + periodsNeeded * periodMs - now
    if retryMs < 1 then
        retryMs = 1
    end
end

local current = math.floor(tokens)
local remaining = tokenLimit - current
if remaining < 0 then
    remaining = 0
end

return { allowed, current, remaining, retryMs }
