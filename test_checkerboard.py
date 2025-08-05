# matplotlib 없이 실행

def paint_floor_tiles_2stage(floor_positions):
    """2단계 처리 방식 (완전한 3x3 블록 + 나머지)"""
    pattern_size = 3
    floor_set = set(floor_positions)
    processed_positions = set()
    result = {}
    
    # 1단계: 완전한 3x3 블록부터 처리
    for pos in floor_positions:
        x, y = pos
        if pos in processed_positions:
            continue
            
        # 이 위치가 속한 3x3 블록의 시작점 계산
        block_start_x = (x // pattern_size) * pattern_size
        block_start_y = (y // pattern_size) * pattern_size
        
        # 3x3 블록이 완전히 존재하는지 확인
        is_complete_block = True
        for dx in range(pattern_size):
            for dy in range(pattern_size):
                check_pos = (block_start_x + dx, block_start_y + dy)
                if check_pos not in floor_set:
                    is_complete_block = False
                    break
            if not is_complete_block:
                break
        
        if is_complete_block:
            # 완전한 3x3 블록인 경우
            block_x = x // pattern_size
            block_y = y // pattern_size
            is_black = (block_x + block_y) % 2 == 0
            color = 'black' if is_black else 'white'
            
            # 3x3 블록 전체를 같은 색으로 칠하기
            for dx in range(pattern_size):
                for dy in range(pattern_size):
                    block_pos = (block_start_x + dx, block_start_y + dy)
                    result[block_pos] = color
                    processed_positions.add(block_pos)
    
    # 2단계: 남은 불완전한 타일들 처리
    for pos in floor_positions:
        if pos in processed_positions:
            continue
            
        x, y = pos
        block_x = x // pattern_size
        block_y = y // pattern_size
        is_black = (block_x + block_y) % 2 == 0
        color = 'black' if is_black else 'white'
        result[pos] = color
    
    return result

def paint_floor_tiles_simple(floor_positions):
    """현재 TilemapVisualizer 방식 (x+y)%2"""
    result = {}
    for pos in floor_positions:
        x, y = pos
        is_black = (x + y) % 2 == 0
        color = 'black' if is_black else 'white'
        result[pos] = color
    return result

# 테스트용 바닥 타일 생성 (불규칙한 형태)
def create_test_floor():
    floor = []
    
    # 완전한 3x3 블록들
    for block_x in range(2):
        for block_y in range(2):
            start_x = block_x * 3
            start_y = block_y * 3
            for dx in range(3):
                for dy in range(3):
                    floor.append((start_x + dx, start_y + dy))
    
    # 불완전한 부분들 추가
    incomplete_tiles = [(6, 0), (6, 1), (7, 0), (0, 6), (1, 6), (2, 6), (0, 7)]
    floor.extend(incomplete_tiles)
    
    return floor

# 시각화 함수 (matplotlib 없이)
def visualize_result(result, title):
    if not result:
        print(f"{title}: No tiles to display")
        return
        
    # 좌표 범위 계산
    x_coords = [pos[0] for pos in result.keys()]
    y_coords = [pos[1] for pos in result.keys()]
    
    min_x, max_x = min(x_coords), max(x_coords)
    min_y, max_y = min(y_coords), max(y_coords)

    # 결과 출력
    print(f"\n{title} 결과:")
    for y in range(max_y, min_y - 1, -1):
        row = ""
        for x in range(min_x, max_x + 1):
            if (x, y) in result:
                row += "■" if result[(x, y)] == 'black' else "□"
            else:
                row += "·"
        print(f"y={y:2d}: {row}")
    
    # 3x3 블록 확인
    print(f"\n3x3 블록 분석 ({title}):")
    for block_y in range((min_y // 3), (max_y // 3) + 1):
        for block_x in range((min_x // 3), (max_x // 3) + 1):
            start_x = block_x * 3
            start_y = block_y * 3
            block_tiles = []
            block_colors = []
            
            for dy in range(3):
                for dx in range(3):
                    pos = (start_x + dx, start_y + dy)
                    if pos in result:
                        block_tiles.append(pos)
                        block_colors.append(result[pos])
            
            if block_tiles:
                all_same = len(set(block_colors)) == 1
                print(f"블록 ({block_x},{block_y}): {len(block_tiles)}개 타일, {'통일됨' if all_same else '혼합됨'}, 색상: {set(block_colors)}")

# 테스트 실행
if __name__ == "__main__":
    floor_positions = create_test_floor()
    
    print("바닥 타일 위치:")
    for pos in sorted(floor_positions):
        print(pos, end=" ")
    print(f"\n총 {len(floor_positions)}개 타일")
    
    # 두 방식 비교
    result_2stage = paint_floor_tiles_2stage(floor_positions)
    result_simple = paint_floor_tiles_simple(floor_positions)
    
    visualize_result(result_2stage, "2단계 처리 방식 (완전한 3x3 + 나머지)")
    visualize_result(result_simple, "현재 Unity 방식 (x+y)%2")