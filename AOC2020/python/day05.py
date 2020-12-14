import operator


def solve(data):
    boarding_cards = data.splitlines()
    return solve_part_1(boarding_cards), solve_part_2(boarding_cards)


def bsp_2_row_col(bsp):
    rows = [0, 128]
    for char in bsp[:7]:
        if char == "B":
            rows = [rows[0] + (rows[1] - rows[0]) // 2, rows[1]]
        else:
            rows = [rows[0], rows[1] - (rows[1] - rows[0]) // 2]
    rows = list(range(*rows))
    row = rows[0]
    columns = [0, 8]
    for char in bsp[7:]:
        if char == "R":
            columns = [columns[0] + (columns[1] - columns[0]) // 2, columns[1]]
        else:
            columns = [columns[0], columns[1] - (columns[1] - columns[0]) // 2]
    columns = list(range(*columns))
    column = columns[0]

    return row, column


def solve_part_1(boarding_cards):
    return max(
        [rc[0] * 8 + rc[1] for rc in (bsp_2_row_col(bc) for bc in boarding_cards)]
    )


def solve_part_2(boarding_cards):
    all_seats = sorted(
        [bsp_2_row_col(bc) for bc in boarding_cards], key=operator.itemgetter(0, 1)
    )
    seat_ids = [rc[0] * 8 + rc[1] for rc in all_seats]
    delta = set(range(seat_ids[0], seat_ids[-1] + 1)).difference(seat_ids)
    return delta.pop()


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
