
def solve(data):
    the_map = []
    for row in data.splitlines():
        the_map.append([_ for _ in row])
    return solve_part_1(the_map, 3, 1), solve_part_2(the_map)



def solve_part_1(_map, x_incr, y_incr):
    n_trees = 0
    x,y = 0,0
    row_length = len(_map[0])
    while y < len(_map)-1:
        x += x_incr
        y += y_incr
        n_trees += int(_map[y][x % row_length] == '#')
    return n_trees


def solve_part_2(_map):
    a = solve_part_1(_map, 1, 1)
    b = solve_part_1(_map, 3, 1)
    c = solve_part_1(_map, 5, 1)
    d = solve_part_1(_map, 7, 1)
    e = solve_part_1(_map, 1, 2)
    return a*b*c*d*e


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
