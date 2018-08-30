import {Format} from "../format";
import {Category} from "../category.model";
import {Limit} from "../limit.model";
import {SubCategory} from "../subcategory.model";
import {Attribute} from "../attribute.model";
import {Type} from "../type.model";

export class DeckCardSearchModel {
  format: Format;
  category: Category;
  subCategory: SubCategory;
  attribute: Attribute;
  type: Type;
  lvlRank: string;
  limit: Limit;
  atk: number;
  def: number;
  search: string;

  constructor(values: Object = {}) {
    Object.assign(this, values);
  }
}
