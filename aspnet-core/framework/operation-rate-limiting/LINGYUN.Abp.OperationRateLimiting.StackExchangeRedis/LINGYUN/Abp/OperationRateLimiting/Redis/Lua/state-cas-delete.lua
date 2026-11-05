-- Compare-and-delete of a custom state value.
-- KEYS[1] = state key
-- ARGV[1] = expected value
-- ARGV[2] = expected exists (1/0)
-- returns 1 when the key was deleted, 0 on a version conflict

local expected = ARGV[1]
local expectedExists = ARGV[2] == '1'

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

redis.call('DEL', KEYS[1])
return 1
