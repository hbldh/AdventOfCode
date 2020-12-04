from itertools import combinations


def solve(data):
    values = [int(x) for x in data.splitlines()]
    return solve_part_1(values), solve_part_2(values)


def solve_part_1(values):
    sorted_values = sorted(values)
    for k in sorted_values:
        v = list(filter(lambda x: k + x == 2020, sorted_values))
        if v:
            return k * v[0]


def solve_part_2(values):
    for triplet in combinations(sorted(values), 3):
        if sum(triplet) == 2020:
            return triplet[0] * triplet[1] * triplet[2]


if __name__ == "__main__":
    from AOC2020.python import run_solver
    run_solver(solve, __file__)
