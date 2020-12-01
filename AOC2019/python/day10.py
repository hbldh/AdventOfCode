import fractions
import itertools
import math

def solve(data):
    astroid_map = [[x == "#" for x in row] for row in data.splitlines()]
    solution_1_value, pos = solve_part_1(astroid_map)
    return solution_1_value, solve_part_2(astroid_map, pos)


def solve_part_1(astroid_map):
    astroid_points = [
        [(col_nbr, row_nbr) for col_nbr, value in enumerate(row) if value]
        for row_nbr, row in enumerate(astroid_map)
    ]
    astroid_dict = {x: True for x in itertools.chain(*astroid_points)}
    n_observable = {}
    for ap in astroid_dict:
        n = 0
        for ap2 in filter(lambda x: x != ap, astroid_dict):
            line = list(get_line(ap, ap2))
            x = [astroid_dict.get(v, False) for v in line]
            n += int(not any(x[1:-1]))
        n_observable[ap] = n
    g = sorted(n_observable.items(), key=lambda x: x[1], reverse=True)
    return g[0][1], g[0][0]  # Value first, position after that, for part 2.


def solve_part_2(astroid_map, origo):
    astroid_points = [
        [(col_nbr, row_nbr) for col_nbr, value in enumerate(row) if value]
        for row_nbr, row in enumerate(astroid_map)
    ]
    astroid_dict = {x: True for x in itertools.chain(*astroid_points)}

    def angle_between(inp):
        x1, y1 = (0, -1)
        x2, y2 = inp[0] - origo[0], inp[1] - origo[1]
        a = math.atan2(x1 * y2 - y1 * x2, x1 * x2 + y1 * y2)
        d = math.degrees(a)
        return d if d >= 0 else (2 * math.pi + a) * 360 / (2 * math.pi)

    destroyed = []

    while True:
        ap = origo
        observable = []
        for ap2 in filter(lambda x: x != ap, astroid_dict):
            line = list(get_line(ap, ap2))
            x = [astroid_dict.get(v, False) for v in line]
            if not any(x[1:-1]):
                observable.append(ap2)
        sorted_observable = sorted(observable, key=angle_between)
        for ap2 in sorted_observable:
            astroid_dict.pop(ap2)
            destroyed.append(ap2)
        if len(destroyed) >= 200:
            break
    return destroyed[199][0] * 100 + destroyed[199][1]


def irange(start, stop, step):
    if stop < start and step < 0:
        return range(start, stop - 1, step)
    if stop > start and step > 0:
        return range(start, stop + 1, step)
    return range(start, stop + 1, step)


def get_line(start, end):
    if end[0] == start[0]:
        y_range = irange(start[1], end[1], 1 if start[1] < end[1] else -1)
        return ((end[0], y) for y in y_range)
    elif end[1] == start[1]:
        x_range = irange(start[0], end[0], 1 if start[0] < end[0] else -1)
        return ((x, start[1]) for x in x_range)

    k = fractions.Fraction((end[1] - start[1]), (end[0] - start[0]))
    k_num = k.numerator
    k_denom = k.denominator

    sign = -1 if end[0] < start[0] else 1
    x_step = sign * k_denom
    y_step = sign * k_num

    return (
        (x, y)
        for x, y in zip(
            range(start[0], end[0] + (1 if start[0] < end[0] else -1), x_step),
            range(start[1], end[1] + (1 if start[1] < end[1] else -1), y_step),
        )
    )

if __name__ == "__main__":
    from AOC2019.python import run_solver

    run_solver(solve, __file__)
