from collections import deque

import tqdm


def solve_part_1(cups: list):
    m = min(cups)
    M = max(cups)
    n = 0
    for _ in range(100):
        this_cup = cups[n]
        if n + 4 > M:
            # Will wrap
            picked_up_cups = (cups[(n + 1) :] + cups[: (n + 4) % M])[:3]
            cups_pre = cups[(n + 4) % M : n]
        else:
            picked_up_cups = cups[(n + 1) : n + 4][:3]
            cups_pre = cups[:n]

        cups_post = cups[(n + 4) :]

        destination_cup = this_cup - 1 if (this_cup - 1) >= m else M
        while destination_cup in picked_up_cups:
            destination_cup = destination_cup - 1 if (destination_cup - 1) >= m else M
        cups = (
            cups_pre
            + [
                this_cup,
            ]
            + cups_post
        )
        idx = cups.index(destination_cup)

        cups = cups[: idx + 1] + picked_up_cups + cups[idx + 1 :]
        k = cups.index(this_cup)
        cups = deque(cups)
        cups.rotate(n - k)
        cups = list(cups)
        n = (n + 1) % M

    return cups


def solve_part_2(cups: list):
    m = min(cups)
    M = max(cups)
    n = 0
    for _ in tqdm.tqdm(range(10000000)):
        this_cup = cups[n]
        if n + 4 > M:
            # Will wrap
            picked_up_cups = (cups[(n + 1) :] + cups[: (n + 4) % M])[:3]
            cups_pre = cups[(n + 4) % M : n]
        else:
            picked_up_cups = cups[(n + 1) : n + 4][:3]
            cups_pre = cups[:n]

        cups_post = cups[(n + 4) :]

        destination_cup = this_cup - 1 if (this_cup - 1) >= m else M
        while destination_cup in picked_up_cups:
            destination_cup = destination_cup - 1 if (destination_cup - 1) >= m else M
        cups = (
            cups_pre
            + [
                this_cup,
            ]
            + cups_post
        )
        idx = cups.index(destination_cup)

        cups = cups[: idx + 1] + picked_up_cups + cups[idx + 1 :]
        k = cups.index(this_cup)
        cups = deque(cups)
        cups.rotate(n - k)
        cups = list(cups)
        n = (n + 1) % M

    return cups


def solve(data: str):

    data = "389125467"
    labeling = list(int(d) for d in data)

    part_1 = solve_part_1(labeling.copy())
    part_1 = "".join(map(str, part_1))
    idx = part_1.index("1")
    part_1 = int(part_1[idx + 1 :] + part_1[:idx])

    part_2 = solve_part_2(labeling + list(range(max(labeling), 1000000)))

    return part_1, part_2


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
