import itertools
from collections import deque

from AOC2017 import ensure_data

ensure_data(10)
with open('input_10.txt', 'r') as f:
    data = f.read().strip()

def knot_hash(lengths, size=256):
    skip_size = 0
    position = 0
    d = deque(range(size))
    for l in lengths:
        for v in [d.popleft() for x in range(l)]:
            d.appendleft(v)
        d.rotate(-(l + skip_size))
        position = (position + l + skip_size) % size
        skip_size += 1
    d.rotate(position)
    return list(d)


def knot_hash_2(s, rounds=64):
    skip_size = 0
    position = 0
    lengths = [ord(x) for x in s] + [17, 31, 73, 47, 23]
    d = deque(range(255))
    for x in range(rounds):
        for l in lengths:
            for v in [d.popleft() for x in range(l)]:
                d.appendleft(v)
            d.rotate(-(l + skip_size))
            position = (position + l + skip_size) % len(d)
            skip_size += 1
    d.rotate(position)
    sparse_hash = list(d)
    dense_hash = []
    for block in [sparse_hash[x:x+16] for x in range(0, 256, 16)]:
        value = 0
        for y in block:
            value ^= y
        dense_hash.append(hex(value)[2:].zfill(2))
    return "".join(dense_hash)



hash_1 = knot_hash(map(int, data.split(',')))
#hash_1 = knot_hash([3,4,1,5], 5)
solution_1 = hash_1[0] * hash_1[1]
hash_2 = knot_hash_2('1,2,3')
solution_2 = hash_2

print("Part 1: {0}".format(solution_1))
print("Part 2: {0}".format(solution_2))
