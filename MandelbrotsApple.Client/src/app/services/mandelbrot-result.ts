import { ErrorType } from "./error-type";

export interface MandelbrotResult {
  imageData: string,
  errors: ErrorType[],
  hasErrors: boolean
}
