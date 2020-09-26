if (redis.call('EXISTS', KEYS[1]) == 0) then
    redis.call('SETEX',KEYS[1],ARGV[1], 0)
end
return tonumber(redis.call('GET', KEYS[1]))