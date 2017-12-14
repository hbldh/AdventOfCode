import itertools

from AOC2017 import ensure_data
#from AOC2017.D17 import knot_hash

ensure_data(14)
with open('input_14.txt', 'r') as f:
    data = f.read().strip()

def int2bin(v):
    return bin(v)[2].zfill(4)

solution_1 = 0
solution_2 = 0

print("Part 1: {0}".format(solution_1))
print("Part 2: {0}".format(solution_2))