

def solve_part_1(start_time, busses):
    wait_until_bus = [b - (start_time % b) for b in busses if b]
    idx = wait_until_bus.index(min(wait_until_bus))
    return [b for b in busses if b][idx] * min(wait_until_bus)


def solve_part_2_brute_force(busses):
    def is_valid(idx, i):
        if busses[i]:
            return ((idx + i) % busses[i]) == 0
        else:
            return True
    t = 0  # 867295486378319 - busses[0]
    while True:
        t += busses[0]
        if all((is_valid(t, i) for i in range(1, len(busses)))):
            return t


def solve_part_2(busses):
        t = 0
        step = busses[0]

        for i, bus in filter(lambda x: x[1], enumerate(busses[1:], start=1)):
            while (t + i) % bus != 0:
                t += step
            step *= bus

        return t


def solve(data):
    start_time, busses = data.splitlines()
    start_time = int(start_time)
    busses = [int(x) if x.isdigit() else None for x in busses.split(',')]

    return solve_part_1(start_time, busses), solve_part_2(busses)


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
