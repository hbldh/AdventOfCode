import os
import re
from collections import defaultdict
from itertools import chain


def solve(data):

    d = defaultdict(lambda: defaultdict(lambda: list()))

    for row in data.splitlines():
        n, l_offset, top_offset, w, h = map(int, re.search("\\#(\d+)\\s*@\\s*(\\d+),(\\d+):\\s(\\d+)x(\\d+)", row).groups())
        for y in range(top_offset, top_offset + h):
            for x in range(l_offset, l_offset + w):
                d[y][x].append(n)

    no_overlap_ID = set(range(1, n + 1)).difference(set(chain(*[x for x in list(chain(*[row.values() for row in d.values()])) if len(x) > 1]))).pop()

    return sum([sum([len(x) > 1 for x in row.values()]) for row in d.values()]), no_overlap_ID


def main():
    from AOC2018 import ensure_data

    day = __file__.split(os.sep)[-1].strip('day').strip('.py')

    ensure_data(int(day))

    with open(f'input_{day}.txt', 'r') as f:
        data = f.read().strip()

    part_1, part_2 = solve(data)

    print("Part 1: {0}".format(part_1))
    print("Part 2: {0}".format(part_2))


if __name__ == '__main__':
    main()