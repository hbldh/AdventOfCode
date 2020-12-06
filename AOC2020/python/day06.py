import itertools


def solve(data):
    groups = []
    this_group = []
    for row in data.splitlines():
        if not row:
            groups.append(this_group)
            this_group = []
        else:
            this_group.append([x for x in row])
    groups.append(this_group)

    return solve_part_1(groups), solve_part_2(groups)


def solve_part_1(groups):
    return sum([len(set(itertools.chain(*group))) for group in groups])


def solve_part_2(groups):
    same = []
    for group in groups:
        answers = set(group[0])
        for responder in group[1:]:
            answers = answers.intersection(responder)
        same.append(len(answers))
    return sum(same)


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
