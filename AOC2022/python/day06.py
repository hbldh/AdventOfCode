def sliding_window(values, n):
    start_idx, stop_idx = 0, n
    while True:
        if len(set(values[start_idx:stop_idx])) == n:
            return stop_idx
        else:
            start_idx += 1
            stop_idx = start_idx + n


def solve(data):
    return solve_part_1(data), solve_part_2(data)


def solve_part_1(values):
    return sliding_window(values, 4)


def solve_part_2(values):
    return sliding_window(values, 14)


if __name__ == "__main__":
    from AOC2022.python import run_solver

    run_solver(solve, __file__)
