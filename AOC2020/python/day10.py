from collections import Counter


def solve_part_1(values):
    v = sorted(values)
    v.append(v[-1] + 3)
    v.insert(0, 0)
    diffs = [b - a for a, b in zip(v[:-1], v[1:])]
    c = Counter(diffs)
    return c[3] * c[1]


def solve_part_2(values):
    v = sorted(values)
    v.append(v[-1] + 3)
    v.insert(0, 0)

    paths = {v[-1]: 1}
    for a in v[:-1][::-1]:
        paths[a] = sum(paths.get(x, 0) for x in (a + 1, a + 2, a + 3))
    return paths[0]


def solve(data):
    values = list(map(int, data.splitlines()))
    return solve_part_1(values), solve_part_2(values)


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
