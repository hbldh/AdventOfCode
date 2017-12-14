from itertools import product, groupby
from operator import itemgetter
from collections import deque, Counter, defaultdict

from AOC2017 import ensure_data

ensure_data(14)
with open('input_14.txt', 'r') as f:
    data = f.read().strip()


def knot_hash(s, size=256, rounds=64):
    skip_size = 0
    position = 0
    lengths = [ord(x) for x in s] + [17, 31, 73, 47, 23]
    d = deque(range(size))
    for x in range(rounds):
        for l in lengths:
            for v in [d.popleft() for _ in range(l)]:
                d.appendleft(v)
            d.rotate(-(l + skip_size))
            position = (position + l + skip_size) % size
            skip_size += 1
    d.rotate(position)
    sparse_hash = list(d)
    dense_hash = []
    for block in [sparse_hash[x:x+16] for x in range(0, size, 16)]:
        value = 0
        for y in block:
            value ^= y
        dense_hash.append(hex(value)[2:].zfill(2))
    return "".join(dense_hash)


def build_matrix(indata):
    hashes = [knot_hash(indata + "-" + str(k)) for k in range(128)]
    rows = ["".join([bin(int(y, 16))[2:].zfill(4) for y in x]) for x in hashes]
    return "\n".join(rows)


def connected_components(matrix):
    height, width = len(matrix), len(matrix[0])

    labels = {}
    label_equivalences = defaultdict(set)
    current_label = 1
    # Connected components solver.
    for y, x in product(range(height), range(width)):
        if matrix[y][x]:
            if x > 0 and y > 0 and matrix[y][x - 1] and matrix[y - 1][x]:
                m = min(labels[(x, y - 1)], labels[(x - 1, y)])
                M = max(labels[(x, y - 1)], labels[(x - 1, y)])
                labels[x, y] = m
                label_equivalences[M].add(m)
                label_equivalences[m].add(M)
            elif x > 0 and matrix[y][x - 1]:
                labels[x, y] = labels[(x - 1, y)]
            elif y > 0 and matrix[y - 1][x]:
                labels[x, y] = labels[(x, y - 1)]
            else:
                labels[x, y] = current_label
                current_label += 1

    # Connect labels.
    for label in labels.values():
        for equivalent_label in label_equivalences[label]:
            label_equivalences[label] = label_equivalences[label].union(
                label_equivalences[equivalent_label])
            label_equivalences[equivalent_label] = label_equivalences[
                equivalent_label].union(
                label_equivalences[label])
        label_equivalences[label].add(label)

    for y, x in product(range(height), range(width)):
        if matrix[y][x]:
            label = labels[(x, y)]
            min_label = min(
                [label, ] + list(label_equivalences.get(label, set())))
            labels[(x, y)] = min_label

    regions = sorted([(x[0], tuple([y[0] for y in x[1]])) for x in groupby(
        sorted(labels.items(), key=itemgetter(1)), key=itemgetter(1))])
    return regions


def solve_part_1(indata):
    c = Counter(build_matrix(indata))
    return c['1']


def solve_part_2(indata):
    matrix = [[bool(int(x)) for x in row] for row in
              build_matrix(indata).splitlines()]
    regions = connected_components(matrix)
    return len(regions)


solution_1 = solve_part_1(data)
solution_2 = solve_part_2(data)

print("Part 1: {0}".format(solution_1))
print("Part 2: {0}".format(solution_2))
