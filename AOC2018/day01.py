def solve(data):
    values = list(map(int, data.splitlines()))

    f_reached = set()
    freq = 0
    while True:
        for n in values:
            freq += n
            if freq in f_reached:
                return sum(values), freq
            else:
                f_reached.add(freq)


if __name__ == '__main__':
    from AOC2018 import run_solver
    run_solver(solve, __file__)
