from collections import defaultdict


class Intcode:
    def __init__(self, data, values=None):
        self.prog = defaultdict(int)
        for i, j in enumerate(map(int, data.split(","))):
            self.prog[i] = j
        if values is None:
            self.values = []
        else:
            self.values = values
        self.pos = 0
        self.rel_base = 0
        self.output = None

    def set(self, i, v, mode):
        if mode == "0":
            self.prog[self.prog[self.pos+i]] = v
        else:
            self.prog[self.prog[self.pos+i]+self.rel_base] = v

    def get(self, i, mode):
        if mode == "0":
            return self.prog[self.prog[self.pos+i]]
        elif mode == "1":
            return self.prog[self.pos+i]
        elif mode == "2":
            return self.prog[self.prog[self.pos+i]+self.rel_base]

    def run(self):
        while True:
            instruction = str(self.prog[self.pos]).zfill(5)
            opcode = instruction[3:]
            modes = list(reversed(instruction[:3]))

            a, b = None, None
            try:
                a = self.get(1, modes[0])
                b = self.get(2, modes[1])
            except IndexError:
                pass

            if opcode == "01":
                self.set(3, a + b, modes[2])
                self.pos += 4
            elif opcode == "02":
                self.set(3, a * b, modes[2])
                self.pos += 4
            elif opcode == "03":
                if len(self.values) == 0:
                    yield None
                self.set(1, self.values.pop(0), modes[0])
                self.pos += 2
            elif opcode == "04":
                yield self.get(1, modes[0])
                self.output = self.get(1, modes[0])
                self.pos += 2
            elif opcode == "05":
                self.pos = b if a != 0 else self.pos+3
            elif opcode == "06":
                self.pos = b if a == 0 else self.pos+3
            elif opcode == "07":
                self.set(3, 1 if a < b else 0, modes[2])
                self.pos += 4
            elif opcode == "08":
                self.set(3, 1 if a == b else 0, modes[2])
                self.pos += 4
            elif opcode == "09":
                self.rel_base += a
                self.pos += 2
            else:
                assert opcode == "99"
                return self.output


def inside_beam(data, x, y):
    if x < 0 or y < 0:
        return False

    *_, output = Intcode(data, [x, y]).run()
    return output == 1


def part_1(data):
    return sum(inside_beam(data, x, y) for x in range(50) for y in range(50))


def part_2(data):
    x = 0
    for y in range(100, 100000):
        while not inside_beam(data, x, y):
            x += 1
        if inside_beam(data, x+99, y-99):
            return 10000*x + (y-99)

def solve(data):
    return part_1(data), part_2(data)


if __name__ == "__main__":
    from AOC2019.python import run_solver

    run_solver(solve, __file__)
