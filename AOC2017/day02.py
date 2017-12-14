import itertools

from AOC2017 import ensure_data

ensure_data(2)
with open('input_02.txt', 'r') as f:
    data = f.read().strip()


def row_checksum(r):
    x = list(map(int, r.split('\t')))
    return max(x) - min(x)


def evenly_divisible_numbers_checksum(r):
    x = list(map(int, r.split('\t')))
    a, b = list(filter(lambda y: (y[0] % y[1]) == 0 or (y[1] % y[0]) == 0,
                       itertools.combinations(x, 2)))[0]
    return max(a, b) // min(a, b)


print("Part 1: {0}".format(sum([row_checksum(r) for r in data.splitlines(keepends=False)])))
print("Part 2: {0}".format(sum([evenly_divisible_numbers_checksum(r) for r in data.splitlines(keepends=False)])))