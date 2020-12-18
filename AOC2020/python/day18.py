import operator


def eval_statement(statement):
    x = 0
    current_operator = operator.add
    for s in statement:
        if isinstance(s, int):
            x = current_operator(x, s)
        elif isinstance(s, list):
            x = current_operator(x, eval_statement(s))
        else:
            current_operator = operator.add if s == "+" else operator.mul
    return x


def eval_statement_2(statement):

    while len(statement) > 3 and "+" in statement:
        idx = statement.index("+")
        new_val = eval_statement_2(statement[idx - 1 : idx + 2])
        statement = (
            statement[: idx - 1]
            + [
                new_val,
            ]
            + statement[idx + 2 :]
        )

    x = 0
    current_operator = operator.add
    for s in statement:
        if isinstance(s, int):
            x = current_operator(x, s)
        elif isinstance(s, list):
            x = current_operator(x, eval_statement_2(s))
        else:
            current_operator = operator.add if s == "+" else operator.mul
    return x


def split_statement(s):
    statement = []
    n = 0
    while n < len(s):
        char = s[n]
        if char == " ":
            n += 1
        elif char.isdigit():
            statement.append(int(char))
            n += 1
        elif char == "+":
            statement.append(char)
            n += 1
        elif char == "*":
            statement.append(char)
            n += 1
        elif char == "(":
            n_parens = 1
            n += 1
            start_new_statement = n
            while n_parens:
                char = s[n]
                if char == ")":
                    n_parens -= 1
                elif char == "(":
                    n_parens += 1
                n += 1
            statement.append(split_statement(s[start_new_statement : n - 1]))
            n += 1
        else:
            n += 1
    return statement


def solve_part_1(data):
    x = 0
    for row in data.splitlines():
        x += eval_statement(split_statement(row))
    return x


def solve_part_2(data):
    x = 0
    for row in data.splitlines():
        x += eval_statement_2(split_statement(row))
    return x


def solve(data):
    return solve_part_1(data), solve_part_2(data)


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
