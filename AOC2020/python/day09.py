import itertools


def solve_part_1(values, preamble_length):

    n = preamble_length
    for t in values[preamble_length:]:
        valids = set(
            (
                x[0] + x[1]
                for x in itertools.combinations(values[n - preamble_length : n], 2)
            )
        )
        if t not in valids:
            return t
        n += 1


def solve_part_2(values, sought):
    start, end = 0, 1
    while True:
        while sum(values[start:end]) < sought:
            end += 1
        while sum(values[start:end]) > sought:
            start += 1
        if sum(values[start:end]) == sought:
            return min(values[start:end]) + max(values[start:end])


def solve(data):
    values = tuple(map(int, data.splitlines()))
    part_1 = solve_part_1(values, 25)
    return part_1, solve_part_2(values, part_1)


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
