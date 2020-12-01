def _solve(data, c, n, p, o=0, i=1):
    inv = lambda x: pow(x, c-2, c)
    for s in [s.split() for s in data.splitlines()]:
        if s[0] == 'cut':  o += i * int(s[-1])
        if s[1] == 'with': i *= inv(int(s[-1]))
        if s[1] == 'into': o -= i; i *= -1
    o *= inv(1-i); i = pow(i, n, c)
    return (p*i + (1-i)*o) % c


def solve(data):
    return _solve(data, 10007,10005,2019), _solve(data, 119315717514047,101741582076661,2020)


if __name__ == "__main__":
    from AOC2019.python import run_solver

    run_solver(solve, __file__)