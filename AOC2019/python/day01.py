import math


def solve(data):
    values = [int(x) for x in data.splitlines()]
    return solve_part_1(values), solve_part_2(values)


def solve_part_1(values):
    return sum([int(math.floor(v/3))-2 for v in values])


def solve_part_2(values):
    def mass_calc(v):
        r = int(math.floor(v/3)) - 2
        if r > 0:
            return r + mass_calc(r)
        else:
            return 0
    return sum([mass_calc(v) for v in values])


if __name__ == '__main__':
    from AOC2019.python import run_solver
    run_solver(solve, __file__)
