from math import copysign

MOVES = {
    "L": (0, -1),
    "R": (0, 1),
    "U": (-1, 0),
    "D": (1, 0)
}

TEST_DATA = """R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20"""


def solve(data):
    #data = TEST_DATA
    moves = [x.split(' ') for x in data.splitlines()]
    return solve_part_1(moves), solve_part_2(moves)


def pos_diff(x,y):
    return x[0] - y[0], x[1] - y[1]


def pos_add(x,delta):
    return x[0] + delta[0], x[1] + delta[1]


def solve_part_1(moves):
    visited = set()
    H_pos = (0, 0)
    T_pos = (0, 0)
    visited.add(T_pos)
    for move in moves:
        for _ in range(int(move[1])):
            H_pos = pos_add(H_pos, MOVES[move[0]])
            delta = pos_diff(H_pos, T_pos)
            if abs(delta[0]) <= 1 and abs(delta[1]) <= 1:
                continue
            else:
                # Need to move T_pos
                if all(delta):
                    # It is a diagonal move.
                    T_pos = pos_add(T_pos, (int(copysign(1, delta[0])), int(copysign(1, delta[1]))))
                else:
                    # It is a straight move.
                    if delta[0]:
                        T_pos = pos_add(T_pos, (int(copysign(1, delta[0])), 0))
                    else:
                        T_pos = pos_add(T_pos, (0, int(copysign(1, delta[1]))))
                visited.add(T_pos)
    return len(visited)


def solve_part_2(moves):
    ...


if __name__ == "__main__":
    from AOC2022.python import run_solver

    run_solver(solve, __file__)
