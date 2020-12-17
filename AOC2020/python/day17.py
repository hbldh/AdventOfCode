import itertools
from collections import defaultdict

neighbours_3 = tuple(filter(lambda x: x != (0, 0, 0), list(itertools.product([-1, 0, 1], [-1, 0, 1], [-1, 0, 1]))))
neighbours_4 = tuple(filter(lambda x: x != (0, 0, 0, 0), list(itertools.product([-1, 0, 1], [-1, 0, 1], [-1, 0, 1], [-1, 0, 1]))))


def iterate_3(map):
    new_map = defaultdict(lambda: '.')
    x = [k[0] for k in map.keys()]
    x_start = min(x) - 1
    x_end = max(x) + 1
    y = [k[1] for k in map.keys()]
    y_start = min(y) - 1
    y_end = max(y) + 1
    z = [k[2] for k in map.keys()]
    z_start = min(z) - 1
    z_end = max(z) + 1

    for layer_idx in range(z_start, z_end + 1):
        for row_idx in range(y_start, y_end + 1):
            for column_idx in range(x_start, x_end + 1):
                n_activated_neighbours = 0
                for neighbour in neighbours_3:
                    n_activated_neighbours += int(map[(column_idx + neighbour[0], row_idx + neighbour[1], layer_idx + neighbour[2])] == "#")
                if map[(column_idx, row_idx, layer_idx)] == ".":
                    if n_activated_neighbours == 3:
                        new_map[(column_idx, row_idx, layer_idx)] = '#'
                    else:
                        new_map[(column_idx, row_idx, layer_idx)] = '.'
                else:
                    if 2 <= n_activated_neighbours <= 3:
                        new_map[(column_idx, row_idx, layer_idx)] = '#'
                    else:
                        new_map[(column_idx, row_idx, layer_idx)] = '.'
    return new_map


def iterate_4(map):
    new_map = defaultdict(lambda: '.')
    x = [k[0] for k in map.keys()]
    x_start = min(x) - 1
    x_end = max(x) + 1
    y = [k[1] for k in map.keys()]
    y_start = min(y) - 1
    y_end = max(y) + 1
    z = [k[2] for k in map.keys()]
    z_start = min(z) - 1
    z_end = max(z) + 1
    w = [k[3] for k in map.keys()]
    w_start = min(w) - 1
    w_end = max(w) + 1

    for temporal_idx in range(w_start, w_end + 1):
        for layer_idx in range(z_start, z_end + 1):
            for row_idx in range(y_start, y_end + 1):
                for column_idx in range(x_start, x_end + 1):
                    n_activated_neighbours = 0
                    for neighbour in neighbours_4:
                        n_activated_neighbours += int(map[(column_idx + neighbour[0], row_idx + neighbour[1], layer_idx + neighbour[2], temporal_idx + neighbour[3])] == "#")
                    if map[(column_idx, row_idx, layer_idx, temporal_idx)] == ".":
                        if n_activated_neighbours == 3:
                            new_map[(column_idx, row_idx, layer_idx, temporal_idx)] = '#'
                        else:
                            new_map[(column_idx, row_idx, layer_idx, temporal_idx)] = '.'
                    else:
                        if 2 <= n_activated_neighbours <= 3:
                            new_map[(column_idx, row_idx, layer_idx, temporal_idx)] = '#'
                        else:
                            new_map[(column_idx, row_idx, layer_idx, temporal_idx)] = '.'
    return new_map


def solve_part_1(map):
    for i in range(6):
        map = iterate_3(map)
    return len([v for v in map.values() if v == '#'])


def solve_part_2(map):
    for i in range(6):
        map = iterate_4(map)
    return len([v for v in map.values() if v == '#'])


def solve(data):
    map_3 = defaultdict(lambda: '.')
    map_4 = defaultdict(lambda: '.')
    for y, row in enumerate(data.splitlines()):
        for x, cell in enumerate(row):
            map_3[(x, y, 0)] = cell
            map_4[(x, y, 0, 0)] = cell

    return solve_part_1(map_3), solve_part_2(map_4)


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
