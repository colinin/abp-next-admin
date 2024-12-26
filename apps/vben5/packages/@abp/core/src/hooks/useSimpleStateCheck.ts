import type {
  IHasSimpleStateCheckers,
  ISimpleStateChecker,
  ISimpleStateCheckerSerializer,
} from '../types/global';

import { isNullOrUnDef } from '../utils/is';
import { isNullOrWhiteSpace } from '../utils/string';
import { useRequireAuthenticatedSimpleStateChecker } from './SimpleStateChecking/useRequireAuthenticatedSimpleStateChecker';
import { useRequireFeaturesSimpleStateChecker } from './SimpleStateChecking/useRequireFeaturesSimpleStateChecker';
import { useRequireGlobalFeaturesSimpleStateChecker } from './SimpleStateChecking/useRequireGlobalFeaturesSimpleStateChecker';
import { useRequirePermissionsSimpleStateChecker } from './SimpleStateChecking/useRequirePermissionsSimpleStateChecker';

class SimpleStateCheckerSerializer implements ISimpleStateCheckerSerializer {
  deserialize<TState extends IHasSimpleStateCheckers<TState>>(
    jsonObject: any,
    state: TState,
  ): ISimpleStateChecker<TState> | undefined {
    if (isNullOrUnDef(jsonObject) || !Reflect.has(jsonObject, 'T')) {
      return undefined;
    }
    switch (String(jsonObject.T)) {
      case 'A': {
        return useRequireAuthenticatedSimpleStateChecker();
      }
      case 'F': {
        const features = jsonObject.N as string[];
        if (features === undefined) {
          throw new Error(
            `'N' is not an array in the serialized state checker! JsonObject: ${
              jsonObject
            }`,
          );
        }
        return useRequireFeaturesSimpleStateChecker(
          features,
          jsonObject.A === true,
        );
      }
      case 'G': {
        const globalFeatures = jsonObject.N as string[];
        if (globalFeatures === undefined) {
          throw new Error(
            `'N' is not an array in the serialized state checker! JsonObject: ${
              jsonObject
            }`,
          );
        }
        return useRequireGlobalFeaturesSimpleStateChecker(
          globalFeatures,
          jsonObject.A === true,
        );
      }
      case 'P': {
        const permissions = jsonObject.N as string[];
        if (permissions === undefined) {
          throw new Error(
            `'N' is not an array in the serialized state checker! JsonObject: ${
              jsonObject
            }`,
          );
        }
        return useRequirePermissionsSimpleStateChecker({
          permissions,
          requiresAll: jsonObject.A === true,
          state,
        });
      }
      default: {
        return undefined;
      }
    }
  }

  deserializeArray<TState extends IHasSimpleStateCheckers<TState>>(
    value: string,
    state: TState,
  ): ISimpleStateChecker<TState>[] {
    if (isNullOrWhiteSpace(value)) return [];
    const jsonObject = JSON.parse(value);
    if (isNullOrUnDef(jsonObject)) return [];
    if (Array.isArray(jsonObject)) {
      if (jsonObject.length === 0) return [];
      return jsonObject
        .map((json) => this.deserialize(json, state))
        .filter((checker) => !isNullOrUnDef(checker))
        .map((checker) => checker);
    }
    const stateChecker = this.deserialize(jsonObject, state);
    if (!stateChecker) return [];
    return [stateChecker];
  }

  serialize<TState extends IHasSimpleStateCheckers<TState>>(
    checker: ISimpleStateChecker<TState>,
  ): string | undefined {
    return checker.serialize();
  }

  serializeArray<TState extends IHasSimpleStateCheckers<TState>>(
    stateCheckers: ISimpleStateChecker<TState>[],
  ): string | undefined {
    if (stateCheckers.length === 0) return undefined;
    if (stateCheckers.length === 1) {
      const stateChecker = stateCheckers[0];
      const single = stateChecker?.serialize();
      if (isNullOrUnDef(single)) return undefined;
      return `[${single}]`;
    }
    let serializedCheckers: string = '';
    stateCheckers.forEach((checker) => {
      const serializedChecker = checker.serialize();
      if (!isNullOrUnDef(serializedChecker)) {
        serializedCheckers += `${serializedChecker},`;
      }
    });
    if (serializedCheckers.endsWith(',')) {
      serializedCheckers = serializedCheckers.slice(
        0,
        Math.max(0, serializedCheckers.length - 1),
      );
    }
    return serializedCheckers.length > 0
      ? `[${serializedCheckers}]`
      : undefined;
  }
}

export function useSimpleStateCheck<
  TState extends IHasSimpleStateCheckers<TState>,
>() {
  return {
    serializer: new SimpleStateCheckerSerializer(),
  };
}
