-- KEYS[1] = counter key (sorted set of lease start timestamps)
-- ARGV[1] = now (unix milliseconds)
-- ARGV[2] = lease duration in milliseconds
-- ARGV[3] = permit limit
-- ARGV[4] = consume (1/0)
-- ARGV[5] = unique member (only used when acquiring)
-- ARGV[6] = ttl in milliseconds
-- returns { allowed, current, remaining, retryAfterMs }

local now = tonumber(ARGV[1])
local leaseMs = tonumber(ARGV[2])
local permitLimit = tonumber(ARGV[3])
local consume = ARGV[4] == '1'
local member = ARGV[5]
local ttlMs = tonumber(ARGV[6])

redis.call('ZREMRANGEBYSCORE', KEYS[1], '-inf', now - leaseMs)

local count = redis.call('ZCARD', KEYS[1])
local allowed = 0

if count < permitLimit then
    allowed = 1
    if consume then
        redis.call('ZADD', KEYS[1], now, member)
        count = count + 1
    end
end

local retryMs = 0

if allowed == 0 and count > 0 then
    local oldest = redis.call('ZRANGE', KEYS[1], 0, 0, 'WITHSCORES')
    if oldest and #oldest >= 2 then
        retryMs = tonumber(oldest[2]) + leaseMs - now
        if retryMs < 1 then
            retryMs = 1
        end
    end
end

if count > 0 then
    redis.call('PEXPIRE', KEYS[1], ttlMs)
end

local remaining = permitLimit - count
if remaining < 0 then
    remaining = 0
end

return { allowed, count, remaining, retryMs }
