-- KEYS[1] = counter key
-- ARGV[1] = max count
-- ARGV[2] = consume (1/0)
-- ARGV[3] = ttl in milliseconds (time left in the current window)
-- ARGV[4] = retry after in milliseconds
-- returns { allowed, current, remaining, retryAfterMs }

local maxCount = tonumber(ARGV[1])
local consume = ARGV[2] == '1'
local ttlMs = tonumber(ARGV[3])
local retryMs = tonumber(ARGV[4])

local current = redis.call('GET', KEYS[1])
local count = 0
if current then
    count = tonumber(current)
end

local allowed = 0

if consume then
    if count < maxCount then
        count = count + 1
        redis.call('SET', KEYS[1], count, 'PX', ttlMs)
        allowed = 1
    end
else
    if count < maxCount then
        allowed = 1
    end
end

local remaining = maxCount - count
if remaining < 0 then
    remaining = 0
end

return { allowed, count, remaining, retryMs }
