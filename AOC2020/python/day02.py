import re
from collections import Counter


r = re.compile("([0-9]+)-([0-9]+) ([a-z]+): ([a-zA-Z]*)")


def solve(data):
    values = []
    for row in data.splitlines():
        a, b, char, pwd = r.search(row).groups()
        values.append((pwd, char, int(a), int(b)))
    return solve_part_1(values), solve_part_2(values)


def is_valid_1(password, char, at_least, at_most):
    c = Counter(password)
    if char in c:
        return at_least <= c[char] <= at_most
    else:
        return at_least <= 0 <= at_most


def is_valid_2(password, char, index_1, index_2):
    return (password[index_1 - 1] == char) != (password[index_2 - 1] == char)


def solve_part_1(values):
    return len(list(filter(lambda x: is_valid_1(*x), values)))


def solve_part_2(values):
    return len(list(filter(lambda x: is_valid_2(*x), values)))


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
