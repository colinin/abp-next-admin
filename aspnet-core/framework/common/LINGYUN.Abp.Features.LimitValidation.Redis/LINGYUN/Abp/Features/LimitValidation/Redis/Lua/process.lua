if (redis.call('EXISTS',KEYS[1]) ~= 0) then
	redis.call('INCRBY',KEYS[1], 1)
else
	redis.call('SET',KEYS[1],1, 'EX', ARGV[1])
end
return tonumber(redis.call('GET',KEYS[1]))