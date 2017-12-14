from AOC2017 import ensure_data

ensure_data(1)
with open('input_01.txt', 'r') as f:
    data = f.read().strip()

print("Part 1: {0}".format(sum([int(i) for i, j in zip(data, data[1:] + data[0]) if i == j])))
print("Part 2: {0}".format(sum([int(i) for i, j in zip(data, data[len(data)//2:] + data[:len(data)//2]) if i == j])))