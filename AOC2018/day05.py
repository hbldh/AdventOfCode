import datetime
from collections import defaultdict, Counter
import re
import operator

regexp = re.compile(r"(.)\1")


def perform_iteration(values):
    last_index = 0
    new_values = []
    for v in regexp.finditer(values.lower(), re.IGNORECASE):
        if values[v.start()] == values[v.end()]:
            # Same case. Continue.
            new_values.append(values[last_index:v.end()])
            last_index = v.end()
        else:
            # Different case. Remove.
            new_values.append(values[last_index:v.start()])
            last_index = v.end()

    new_values.append(values[last_index:])
    return "".join(new_values)


def solve(data):
    s1 = solve_part_1(data)


    return len(s1), 0


def solve_part_1(data):
    while True:
        new_data = perform_iteration(data)
        if len(new_data) == len(data):
            break
        else:
            data = new_data
    return new_data


if __name__ == '__main__':
    from AOC2018 import run_solver
    run_solver(solve, __file__)
