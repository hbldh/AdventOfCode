import re


def expand_rule(rules, no, depth) -> str:
    if depth > 15:
        return ""
    rule = rules[no]
    if isinstance(rule, str):
        return rule

    if len(rule) == 2:
        first_part = "".join([f"{expand_rule(rules, r, depth+1)}" for r in rule[0]])
        second_part = "".join([f"{expand_rule(rules, r, depth+1)}" for r in rule[1]])
        return f"({first_part}|{second_part})"
    else:
        return "".join([f"{expand_rule(rules, r, depth+1)}" for r in rule[0]])


def _solve(rules, messages):
    rule_0 = expand_rule(rules, 0, 0)
    rule_0_regex = re.compile(rule_0)
    n = 0
    for message in messages:
        match = rule_0_regex.match(message)
        if match and (match.end() - match.start()) == len(message):
            n += 1
    return n


def solve(data):
    rules = {}
    messages = []
    for row in data.splitlines():
        if ":" in row:
            rule_nbr, statement = row.split(":")
            if '"' in statement:
                rules[int(rule_nbr)] = statement.strip().strip('"')
            else:
                rules[int(rule_nbr)] = tuple(
                    map(
                        lambda x: [int(y) for y in x.strip().split(" ")],
                        statement.split("|"),
                    )
                )
        else:
            messages.append(row)

    part_1 = _solve(rules, messages)

    rules[8] = ((42,), (42, 8))
    rules[11] = ((42, 31), (42, 11, 31))

    part_2 = _solve(rules, messages)

    return part_1, part_2


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
