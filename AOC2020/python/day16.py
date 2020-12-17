def solve_part_1(rules, nearby_tickets):
    all_valid_values = set()
    for values in rules.values():
        all_valid_values.update(list(range(values[0][0], values[0][1] + 1)))
        all_valid_values.update(list(range(values[1][0], values[1][1] + 1)))
    s = 0
    for ticket in nearby_tickets:
        delta = set(ticket).difference(all_valid_values)
        s += sum(delta)
    return s


def solve_part_2(rules, my_ticket, nearby_tickets):
    all_valid_values = set()
    for values in rules.values():
        all_valid_values.update(list(range(values[0][0], values[0][1] + 1)))
        all_valid_values.update(list(range(values[1][0], values[1][1] + 1)))
    valid_tickets = []
    for ticket in nearby_tickets:
        if len(set(ticket).difference(all_valid_values)) == 0:
            valid_tickets.append(ticket)

    field_values = []
    for i in range(len(my_ticket)):
        field_values.append(set([x[i] for x in valid_tickets]))

    known_rules = {}
    rule_ranges = {}
    for rule_name, values in rules.items():
        valid_values = set()
        valid_values.update(list(range(values[0][0], values[0][1] + 1)))
        valid_values.update(list(range(values[1][0], values[1][1] + 1)))
        rule_ranges[rule_name] = valid_values

    while len(known_rules) != len(my_ticket):
        possible_fields = {i: [] for i in range(len(my_ticket))}
        for i in range(len(my_ticket)):
            if i in known_rules.values():
                continue
            for rule, rule_range in rule_ranges.items():
                if rule in known_rules:
                    continue
                if (
                    len(field_values[i].difference(rule_range)) == 0
                    and rule not in known_rules
                ):
                    possible_fields[i].append(rule)
            if len(possible_fields[i]) == 1:
                known_rules[possible_fields[i][0]] = i

    departure_values = []
    for rule in known_rules:
        if rule.startswith("departure"):
            departure_values.append(my_ticket[known_rules[rule]])

    d = departure_values[0]
    for dd in departure_values[1:]:
        d *= dd

    return d


def solve(data):
    raw_rules, tickets = data.split("your ticket:")
    rules = {}
    for row in raw_rules.strip().splitlines():
        field_name, field_values = row.split(":")
        values = [tuple(map(int, r.split("-"))) for r in field_values.split(" or ")]
        rules[field_name.strip()] = values

    def parse_ticket(t):
        return tuple(map(int, t.strip().split(",")))

    my_ticket, nearby_tickets = tickets.split("nearby tickets:")
    my_ticket = parse_ticket(my_ticket)

    nearby_tickets = [parse_ticket(t) for t in nearby_tickets.strip().splitlines()]
    return solve_part_1(rules, nearby_tickets), solve_part_2(
        rules, my_ticket, nearby_tickets
    )


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
