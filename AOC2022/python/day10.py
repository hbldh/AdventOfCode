def solve(data):
    ops = [x.split(' ') for x in data.splitlines()]
    return solve_part_1(ops), solve_part_2(ops)


def run_program(ops):
    vals = [1, ]
    for op in ops:
        match op[0]:
            case "noop":
                vals.append(vals[-1])
            case "addx":
                vals.append(vals[-1])
                vals.append(vals[-1] + int(op[1]))
    return vals


def solve_part_1(ops):
    vals = run_program(ops)
    return sum([vals[i-1] * i for i in range(20, 260, 40)])


def solve_part_2(ops):
    vals = run_program(ops)
    image = ["\n"]
    for position, val in enumerate(vals, start=0):
        if val in [(position - 1) % 40, position % 40, (position + 1) % 40]:
            image.append("#")
        else:
            image.append('.')
        if (position + 1) % 40 == 0:
            image.append("\n")
    return "".join(image)


if __name__ == "__main__":
    from AOC2022.python import run_solver

    run_solver(solve, __file__)
