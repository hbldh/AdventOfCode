from itertools import groupby


def solve(data):
    values = [int(x) if x.isdigit() else None for x in data.splitlines()]
    elves = [list(g) for _, g in groupby(values, lambda s: s is not None)][::2]
    return solve_part_1(elves), solve_part_2(elves)


def solve_part_1(elves):
    return max(map(sum, elves))


def solve_part_2(elves):
    return sum(sorted(map(sum, elves), reverse=True)[:3])


if __name__ == "__main__":
    from AOC2022.python import run_solver

    run_solver(solve, __file__)
