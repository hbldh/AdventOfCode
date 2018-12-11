import os
from collections import Counter
import difflib


def _checksum(r):
    counter = Counter(r)
    return int(any([x == 2 for x in counter.values()])), int(any([x == 3 for x in counter.values()]))


def _solve_1(rows):

    d = [_checksum(row) for row in rows]
    return sum([x[0] for x in d]) * sum([x[1] for x in d]),


def _solve_2(rows):
    for i, r in enumerate(rows):
        for r2 in rows[i:]:
            diffs = len(r) - int(round(difflib.SequenceMatcher(a=r, b=r2).ratio() * len(r)))
            if diffs == 1:
                return "".join([a for a, b in zip(r, r2) if a == b])
    return 0


def solve(data):
    rows = data.splitlines()
    return _solve_1(rows), _solve_2(rows)


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
