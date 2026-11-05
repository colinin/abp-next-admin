-- Compare-and-set of a custom state value.
-- KEYS[1] = state key
-- ARGV[1] = expected value (empty string when the key must not exist)
-- ARGV[2] = expected exists (1/0)
-- ARGV[3] = new value
-- ARGV[4] = ttl in milliseconds
-- returns 1 when the value was written, 0 on a version conflict

local expected = ARGV[1]
local expectedExists = ARGV[2] == '1'
local newValue = ARGV[3]
local ttlMs = tonumber(ARGV[4])

local current = redis.call('GET', KEYS[1])

if expectedExists then
    if current ~= expected then
        return 0
    end
else
    if current then
        return 0
    end
end

redis.call('SET', KEYS[1], newValue, 'PX', ttlMs)
return 1
