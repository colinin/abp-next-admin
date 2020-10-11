if (redis.call('EXISTS', KEYS[1]) == 0) then
    return 0
end
return tonumber(redis.call('GET', KEYS[1]))