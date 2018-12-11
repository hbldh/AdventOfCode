from collections import Counter
import difflib


def _checksum(r):
    counter = Counter(r)
    return int(any([x == 2 for x in counter.values()])), int(any([x == 3 for x in counter.values()]))


def _solve_1(rows):

    d = [_checksum(row) for row in rows]
    return sum([x[0] for x in d]) * sum([x[1] for x in d])


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


if __name__ == '__main__':
    from AOC2018 import run_solver
    run_solver(solve, __file__)
