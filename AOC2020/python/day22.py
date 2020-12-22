from collections import deque
from itertools import islice


def solve_part_1(p1: deque, p2: deque):

    while len(p1) and len(p2):
        _p1, _p2 = p1.popleft(), p2.popleft()
        if _p1 > _p2:
            p1.append(_p1)
            p1.append(_p2)
        else:
            p2.append(_p2)
            p2.append(_p1)
    winner = p1 if len(p1) else p2
    return sum([a * b for a, b in zip(range(len(winner), 0, -1), winner)])


def solve_part_2(p1: deque, p2: deque):
    history = set()
    while len(p1) and len(p2):
        if (tuple(p1), tuple(p2)) in history:
            return 1, p1
        else:
            history.add((tuple(p1), tuple(p2)))
        _p1, _p2 = p1.popleft(), p2.popleft()

        if len(p1) >= _p1 and len(p2) >= _p2:
            w, _ = solve_part_2(deque(islice(p1, _p1)), deque(islice(p2, _p2)))
        elif _p1 > _p2:
            w = 1
        else:
            w = 2

        if w == 1:
            p1.append(_p1)
            p1.append(_p2)
        else:
            p2.append(_p2)
            p2.append(_p1)

    if len(p1) == 0:
        winner = (2, p2)
    else:
        winner = (1, p1)
    return winner


def solve(data: str):
    player_1, player_2 = data.split("\nPlayer 2:\n")
    player_1 = deque(map(int, player_1.strip("Player 1:").strip().splitlines()))
    player_2 = deque(map(int, player_2.strip().splitlines()))

    part_1 = solve_part_1(player_1.copy(), player_2.copy())
    _, winning_deck = solve_part_2(player_1.copy(), player_2.copy())

    part_2 = sum([a * b for a, b in zip(range(len(winning_deck), 0, -1), winning_deck)])
    return part_1, part_2


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
