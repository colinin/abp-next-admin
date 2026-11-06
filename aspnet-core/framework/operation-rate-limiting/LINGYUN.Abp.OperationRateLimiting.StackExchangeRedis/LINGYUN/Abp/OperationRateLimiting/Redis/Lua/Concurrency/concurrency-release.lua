-- KEYS[1] = counter key (sorted set of lease start timestamps)
-- ARGV[1] = now (unix milliseconds)
-- ARGV[2] = lease duration in milliseconds
-- returns the remaining number of active leases

local now = tonumber(ARGV[1])
local leaseMs = tonumber(ARGV[2])

redis.call('ZREMRANGEBYSCORE', KEYS[1], '-inf', now - leaseMs)

local first = redis.call('ZRANGE', KEYS[1], 0, 0)
if first and #first > 0 then
    redis.call('ZREM', KEYS[1], first[1])
end

local count = redis.call('ZCARD', KEYS[1])
if count == 0 then
    redis.call('DEL', KEYS[1])
end

return count
