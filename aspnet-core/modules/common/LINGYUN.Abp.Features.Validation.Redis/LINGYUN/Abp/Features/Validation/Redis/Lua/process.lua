if (redis.call('EXISTS',KEYS[1]) ~= 0) then
	redis.call('INCRBY',KEYS[1], 1)
else
	redis.call('SETEX',KEYS[1],ARGV[1],1)
end
return tonumber(redis.call('GET',KEYS[1]))