def solve(data):
    elves = [row.split(",") for row in data.splitlines()]
    return solve_part_1(elves), solve_part_2(elves)


def _range(a, b):
    return range(a, b + 1)


def make_elf_set(elf):
    return set(_range(*map(int, elf.split("-"))))


def elf_diff(elf_pair):
    elf_set_1, elf_set_2 = map(make_elf_set, (elf_pair[0], elf_pair[1]))
    return (
        len(elf_set_1.difference(elf_set_2)) == 0
        or len(elf_set_2.difference(elf_set_1)) == 0
    )


def elf_intersection(elf_pair):
    elf_set_1, elf_set_2 = map(make_elf_set, (elf_pair[0], elf_pair[1]))
    return (
        len(elf_set_1.intersection(elf_set_2)) > 0
        or len(elf_set_2.intersection(elf_set_1)) > 0
    )


def solve_part_1(elves):
    return sum(map(elf_diff, elves))


def solve_part_2(elves):
    return sum(map(elf_intersection, elves))


if __name__ == "__main__":
    from AOC2022.python import run_solver

    run_solver(solve, __file__)
