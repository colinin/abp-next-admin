export type FormulaMode = 'date-diff' | 'date-range' | 'number';
export type FormulaUnit =
  'day' | 'hour' | 'minute' | 'month' | 'second' | 'year';

export interface FormulaConfig {
  left: string;
  mode: FormulaMode;
  operator: '*' | '+' | '-' | '/';
  right?: string;
  unit?: FormulaUnit;
}

export const formulaModeOptions = [
  { label: '两个数字字段运算', value: 'number' },
  { label: '两个日期字段运算', value: 'date-diff' },
  { label: '日期区间运算', value: 'date-range' },
];

export const formulaOperatorOptions = [
  { label: '加 +', value: '+' },
  { label: '减 -', value: '-' },
  { label: '乘 ×', value: '*' },
  { label: '除 ÷', value: '/' },
];

export const formulaUnitOptions = [
  { label: '年', value: 'year' },
  { label: '月', value: 'month' },
  { label: '日', value: 'day' },
  { label: '时', value: 'hour' },
  { label: '分', value: 'minute' },
  { label: '秒', value: 'second' },
];

export function isFormulaMode(value: unknown): value is FormulaMode {
  return value === 'number' || value === 'date-diff' || value === 'date-range';
}

export function parseFormula(
  optionsJson?: null | string,
): FormulaConfig | null {
  if (!optionsJson) {
    return null;
  }
  try {
    const parsed = JSON.parse(optionsJson) as {
      formula?: Partial<FormulaConfig>;
    };
    const formula = parsed?.formula;
    if (!formula || !isFormulaMode(formula.mode)) {
      return null;
    }
    return {
      left: formula.left ?? '',
      mode: formula.mode,
      operator: formula.operator ?? '+',
      right: formula.right ?? '',
      unit: formula.unit ?? 'day',
    };
  } catch {
    return null;
  }
}

export function serializeFormula(
  formula?: FormulaConfig | null,
): null | string {
  return formula ? JSON.stringify({ formula }) : null;
}

export function calculateFormula(
  formula: FormulaConfig | null | undefined,
  values: Record<string, unknown>,
): null | number | string {
  if (!formula) {
    return null;
  }

  if (formula.mode === 'number') {
    const left = Number(values[formula.left]);
    const right = Number(values[formula.right ?? '']);
    if (!Number.isFinite(left) || !Number.isFinite(right)) {
      return null;
    }
    let result: number;
    switch (formula.operator) {
      case '*': {
        result = left * right;
        break;
      }
      case '+': {
        result = left + right;
        break;
      }
      case '-': {
        result = left - right;
        break;
      }
      case '/': {
        if (right === 0) {
          return null;
        }
        result = left / right;
        break;
      }
      default: {
        return null;
      }
    }
    return Math.round(result * 100) / 100;
  }

  if (formula.mode === 'date-diff') {
    const start = toDate(values[formula.left]);
    const end = toDate(values[formula.right ?? '']);
    if (!start || !end) {
      return null;
    }
    return diffDate(start, end, formula.unit ?? 'day');
  }

  const range = values[formula.left];
  if (!Array.isArray(range) || range.length < 2) {
    return null;
  }
  const start = toDate(range[0]);
  const end = toDate(range[1]);
  if (!start || !end) {
    return null;
  }
  return diffDate(start, end, formula.unit ?? 'day');
}

function toDate(value: unknown): Date | null {
  if (!value) {
    return null;
  }
  const date = new Date(value as Date | number | string);
  return Number.isNaN(date.getTime()) ? null : date;
}

function diffDate(start: Date, end: Date, unit: FormulaUnit): number {
  const millis = end.getTime() - start.getTime();
  switch (unit) {
    case 'hour': {
      return Math.floor(millis / 3_600_000);
    }
    case 'minute': {
      return Math.floor(millis / 60_000);
    }
    case 'month': {
      let months =
        (end.getFullYear() - start.getFullYear()) * 12 +
        (end.getMonth() - start.getMonth());
      if (end.getDate() < start.getDate()) {
        months -= 1;
      }
      return months;
    }
    case 'second': {
      return Math.floor(millis / 1000);
    }
    case 'year': {
      let years = end.getFullYear() - start.getFullYear();
      if (
        end.getMonth() < start.getMonth() ||
        (end.getMonth() === start.getMonth() && end.getDate() < start.getDate())
      ) {
        years -= 1;
      }
      return years;
    }
    default: {
      return Math.floor(millis / 86_400_000);
    }
  }
}
