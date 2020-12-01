from itertools import cycle, accumulate


def solve_part2(data):
        s = data.strip()
        offset = int(s[:7])
        digits = [int(i) for i in s]
        # If `rep` is `digits` repeated 10K times, construct:
        #     arr = [rep[-1], rep[-2], ..., rep[offset]]
        l = 10000 * len(digits) - offset
        i = cycle(reversed(digits))
        arr = [next(i) for _ in range(l)]
        # Repeatedly take the partial sums mod 10
        for _ in range(100):
            arr = [n % 10 for n in accumulate(arr)]
        return "".join(str(i) for i in arr[-1:-9:-1])

def solve(data):
    return 0, solve_part2(data)


if __name__ == "__main__":
    from AOC2019.python import run_solver

    run_solver(solve, __file__)
