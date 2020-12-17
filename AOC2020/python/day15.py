from collections import defaultdict


def find_nth_number(n, history, spoken):
    for i in range(len(spoken) + 1, n + 1):
        if len(history[spoken[-1]]) == 1:
            # First time spoken
            history[0].append(i)
            spoken.append(0)
        else:
            # Find last time spoken.
            last_i = history[spoken[-1]][-2]
            age = history[spoken[-1]][-1] - last_i
            history[age].append(i)
            spoken.append(age)

    return spoken[-1]


def solve_part_1(nbrs):
    history = defaultdict(lambda: list())
    spoken = nbrs
    for i, nbr in enumerate(nbrs):
        history[nbr] = [
            i + 1,
        ]

    return find_nth_number(2020, history, spoken)


def solve_part_2(nbrs):
    history = defaultdict(lambda: list())
    spoken = nbrs
    for i, nbr in enumerate(nbrs):
        history[nbr] = [
            i + 1,
        ]

    return find_nth_number(30000000, history, spoken)


def solve(data):
    starting_numbers = list(map(int, data.split(",")))
    return solve_part_1(starting_numbers), solve_part_2(starting_numbers)


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
