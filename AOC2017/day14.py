from itertools import product
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
    rows = ["".join([bin(int(x, 16))[2:].zfill(4) for x in
                     knot_hash(indata + "-" + str(k))]) for k in range(128)]
    return "\n".join(rows)


def calculate_used_squares(indata):
    c = Counter(build_matrix(indata))
    return c['1']


def calculate_n_regions(indata):
    matrix = [[bool(int(x)) for x in row] for row in
              build_matrix(indata).splitlines()]
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
            label_equivalences[equivalent_label] = label_equivalences[equivalent_label].union(
                label_equivalences[label])
        label_equivalences[label].add(label)

    for y, x in product(range(height), range(width)):
        if matrix[y][x]:
            label = labels[(x, y)]
            min_label = min([label, ] + list(label_equivalences.get(label, set())))
            labels[(x, y)] = min_label

    # Print solution for debugging.
    # for y, x in product(range(height), range(width)):
    #     if matrix[y][x]:
    #         matrix[y][x] = '{0:04d}'.format(labels[(x, y)])
    #     else:
    #         matrix[y][x] = '    '
    # for y in range(height):
    #     print(" ".join(matrix[y]))

    return len(set(labels.values()))

solution_1 = calculate_used_squares(data)
solution_2 = calculate_n_regions(data)

print("Part 1: {0}".format(solution_1))
print("Part 2: {0}".format(solution_2))
