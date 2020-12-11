import copy


def _iterate_part_1(s):
    new_s = copy.deepcopy(s)
    for row, col in new_s.keys():
        if s[(row, col)] == ".":
            continue
        n_occupied = 0
        for comb in (
            (-1, -1),
            (-1, 0),
            (-1, 1),
            (0, -1),
            (0, 1),
            (1, -1),
            (1, 0),
            (1, 1),
        ):
            n_occupied += s.get((row + comb[0], col + comb[1]), ".") == "#"

        if s[(row, col)] == "#" and n_occupied > 3:
            new_s[(row, col)] = "L"
        elif s[(row, col)] == "L" and n_occupied == 0:
            new_s[(row, col)] = "#"
    return new_s


def _iterate_part_2(s):
    new_s = copy.deepcopy(s)
    for row, col in new_s.keys():
        if s[(row, col)] == ".":
            continue
        n_occupied = 0
        for comb in (
            (-1, -1),
            (-1, 0),
            (-1, 1),
            (0, -1),
            (0, 1),
            (1, -1),
            (1, 0),
            (1, 1),
        ):
            d = 1
            while s.get((row + d * comb[0], col + d * comb[1]), None) != None:
                if s.get((row + d * comb[0], col + d * comb[1]), None) == "#":
                    n_occupied += 1
                    break
                elif s.get((row + d * comb[0], col + d * comb[1]), None) == "L":
                    break
                d += 1
        if s[(row, col)] == "#" and n_occupied > 4:
            new_s[(row, col)] = "L"
        elif s[(row, col)] == "L" and n_occupied == 0:
            new_s[(row, col)] = "#"
    return new_s


def solve_part_1(seatings):
    n = 0
    last_x = sum([x == "#" for x in seatings.values()])
    while True:
        seatings = _iterate_part_1(seatings)
        n += 1
        x = sum([x == "#" for x in seatings.values()])
        if x == last_x:
            break
        else:
            last_x = x
    return x


def solve_part_2(seatings):
    n = 0
    last_x = sum([x == "#" for x in seatings.values()])
    while True:
        seatings = _iterate_part_2(seatings)
        n += 1
        x = sum([x == "#" for x in seatings.values()])
        if x == last_x:
            break
        else:
            last_x = x
    return x


def solve(data):
    seatings = {}
    for row_nbr, row in enumerate(data.splitlines()):
        for column_nbr, cell in enumerate(row):
            seatings[(row_nbr, column_nbr)] = cell
    return solve_part_1(seatings), solve_part_2(seatings)


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
