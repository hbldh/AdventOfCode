directions = (1, 0), (0, -1), (-1, 0), (0, 1)  # E,S,W,N


def solve_part_1(instructions):
    x, y, d = 0, 0, 0
    for i in instructions:
        if i[0] == "F":
            x, y = x + directions[d][0] * i[1], y + directions[d][1] * i[1]
        elif i[0] == "N":
            x, y = x + directions[-1][0] * i[1], y + directions[-1][1] * i[1]
        elif i[0] == "S":
            x, y = x + directions[1][0] * i[1], y + directions[1][1] * i[1]
        elif i[0] == "E":
            x, y = x + directions[0][0] * i[1], y + directions[0][1] * i[1]
        elif i[0] == "W":
            x, y = x + directions[2][0] * i[1], y + directions[2][1] * i[1]
        elif i[0] == "R":
            d = (d + (i[1] // 90)) % 4
        elif i[0] == "L":
            d = (d - (i[1] // 90)) % 4
    return abs(x) + abs(y)


def solve_part_2(instructions):
    x, y = 0, 0
    w_x, w_y = 10, 1
    for i in instructions:
        if i[0] == "F":
            x, y = x + w_x * i[1], y + w_y * i[1]
        elif i[0] == "N":
            w_x, w_y = w_x + directions[-1][0] * i[1], w_y + directions[-1][1] * i[1]
        elif i[0] == "S":
            w_x, w_y = w_x + directions[1][0] * i[1], w_y + directions[1][1] * i[1]
        elif i[0] == "E":
            w_x, w_y = w_x + directions[0][0] * i[1], w_y + directions[0][1] * i[1]
        elif i[0] == "W":
            w_x, w_y = w_x + directions[2][0] * i[1], w_y + directions[2][1] * i[1]
        elif i[0] == "R":
            d = (i[1] // 90) % 4
            if d == 1:
                w_x, w_y = w_y, -w_x
            elif d == 2:
                w_x, w_y = -w_x, -w_y
            elif d == 3:
                w_x, w_y = -w_y, w_x
        elif i[0] == "L":
            d = (i[1] // 90) % 4
            if d == 1:
                w_x, w_y = -w_y, w_x
            elif d == 2:
                w_x, w_y = -w_x, -w_y
            elif d == 3:
                w_x, w_y = w_y, -w_x
    return abs(x) + abs(y)


def solve(data):
    instructions = [(x[0], int(x[1:])) for x in data.splitlines()]
    return solve_part_1(instructions), solve_part_2(instructions)


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
