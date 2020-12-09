def parse_boot_code(boot_code):
    parsed_code = []
    for row in boot_code:
        op, value = row.split(" ")
        parsed_code.append((op, int(value)))
    return tuple(parsed_code)


class HandheldGameConsole:
    def __init__(self, boot_code, break_criterion):
        self.boot_code = parse_boot_code(boot_code.splitlines())
        self.i = 0
        self.accumulator = 0
        self.history = {}
        self.break_criterion = break_criterion

    @property
    def terminated_cleanly(self):
        return self.i >= len(self.boot_code)

    def run(self):
        while self.i < len(self.boot_code):
            self.history[self.i] = []
            op, value = self.boot_code[self.i]
            self.i += getattr(self, op)(value)
            if self.break_criterion(self):
                break

    def nop(self, *args) -> int:
        return 1

    def acc(self, value):
        self.accumulator += value
        return 1

    def jmp(self, value):
        return value
