def solve_part_1(pk1, pk2):
    sn = 7
    value = 1
    card_loop_size = 0
    while value != pk2:
        value = (value * sn) % 20201227
        card_loop_size += 1

    encryption_key = 1
    for i in range(card_loop_size):
        encryption_key = (encryption_key * pk1) % 20201227

    return encryption_key


def solve_part_2():
    return 0


def solve(data: str):
    card_public_key, door_public_key = map(lambda x: int(x.strip()), data.splitlines())

    part_1 = solve_part_1(card_public_key, door_public_key)
    part_2 = solve_part_2()
    return part_1, part_2


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
